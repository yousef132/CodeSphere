#!/usr/bin/bash


echo "Script is running..." > /code/debug.log
echo "Current directory contents:" >> /code/debug.log
ls -lah /code >> /code/debug.log
echo "Checking time command:" >> /code/debug.log
which time >> /code/debug.log 2>&1 || echo "time command not found!" >> /code/debug.log


# Ensure `time` is installed (only needed if the base image lacks it)
apt-get update && apt-get install -y time > /dev/null

which time >> /code/debug.log 2>&1 || echo "time command not found!" >> /code/debug.log

# Clear previous logs
: > /code/output.txt
: > /code/runtime.txt
: > /code/error.txt
: > /code/runtime_errors.txt
: > /code/memory.txt  # Clear memory log file

# Timeout duration for each program
TIMEOUT_DURATION=${1:-5s}

# Function to log runtime details
log_runtime_info() {
    local exit_code=$1
    local runtime_info="$2"
    local peak_memory="$3"

    if [[ $exit_code -eq 124 ]]; then
        echo "TIMELIMITEXCEEDED" >> /code/runtime.txt
    elif [[ $exit_code -eq 0 ]]; then
        echo "$runtime_info" >> /code/runtime.txt
        echo "Peak Memory Usage: ${peak_memory} KB" >> /code/memory.txt
    else
        echo "RUNTIMEERROR: Program terminated with exit code $exit_code" >> /code/runtime_errors.txt
    fi
}

# Function to measure memory usage while the program runs
monitor_memory() {
    local pid=$1
    local peak_memory=0
    while [[ -d /proc/$pid ]]; do  # Check if process is still running
        mem_usage=$(grep VmRSS /proc/$pid/status | awk '{print $2}')
        [[ -n "$mem_usage" ]] && (( mem_usage > peak_memory )) && peak_memory=$mem_usage
        sleep 0.05
    done
    echo $peak_memory
}

# Function to execute a program and measure execution time and memory usage
execute_and_measure() {
    local cmd="$1"

    # Start the process in the background and get its PID
    ( /usr/bin/time -f "%e" timeout $TIMEOUT_DURATION $cmd < /code/testcases.txt > /code/output.txt ) 2> /code/runtime.txt &
    pid=$!
    
    # Monitor memory usage in parallel
    peak_memory=$(monitor_memory $pid)

    # Wait for the process to finish and get the exit code
    wait $pid
    exit_code=$?

    # Get runtime from file (time output is stored there)
    runtime_info=$(cat /code/runtime.txt)

    # Log execution details
    log_runtime_info $exit_code "$runtime_info" "$peak_memory"
}

# Execute the appropriate code based on file type
if [[ -f "/code/main.py" ]]; then
    execute_and_measure "python3 /code/main.py"

elif [[ -f "/code/main.cpp" ]]; then
    g++ -o /code/main.out /code/main.cpp 2> /code/error.txt
    if [[ $? -eq 0 ]]; then
        execute_and_measure "/code/main.out"
    else
        echo "COMPILATIONFAILED" >> /code/error.txt
    fi

elif [[ -f "/code/main.cs" ]]; then
    dotnet build /code/main.csproj -o /code/output 2>> /code/error.txt
    if [[ $? -eq 0 ]]; then
        execute_and_measure "dotnet /code/output/main.dll"
    else
        echo "BUILDFAILED" >> /code/error.txt
    fi

else
    echo "UNSUPPORTEDLANGUAGE" >> /code/error.txt
fi
