#!/usr/bin/bash

# Clear previous logs
: > /code/output.txt
: > /code/runtime.txt
: > /code/error.txt
: > /code/runtime_errors.txt

# Timeout duration for each program
TIMEOUT_DURATION=${1:-5s}

# Log runtime details and handle timeouts or runtime errors
log_runtime_info() {
    local exit_code=$1
    local runtime_info="$2"
    
    if [[ $exit_code -eq 124 ]]; then
        echo "TIMELIMITEXCEEDED" >> /code/runtime.txt
    elif [[ $exit_code -eq 0 ]]; then
        echo "$runtime_info" >> /code/runtime.txt
    else
        echo "RUNTIMEERROR: Program terminated with exit code $exit_code" >> /code/runtime_errors.txt
    fi
}

# Execute the appropriate code based on file type
if [[ -f "/code/main.py" ]]; then
    # Run Python code
    runtime_info=$( { time timeout $TIMEOUT_DURATION python3 /code/main.py < /code/testcases.txt > /code/output.txt; } 2>&1 )
    log_runtime_info $? "$runtime_info"

elif [[ -f "/code/main.cpp" ]]; then
    # Compile and run C++ code
    g++ -o /code/main.out /code/main.cpp 2> /code/error.txt
    if [[ $? -eq 0 ]]; then
        runtime_info=$( { time timeout $TIMEOUT_DURATION /code/main.out < /code/testcases.txt > /code/output.txt; } 2>&1 )
        log_runtime_info $? "$runtime_info"
    else
        echo "COMPILATIONFAILED" >> /code/error.txt
    fi

elif [[ -f "/code/main.cs" ]]; then
    # Build and run C# code
    dotnet build /code/main.csproj -o /code/output 2>> /code/error.txt
    if [[ $? -eq 0 ]]; then
        runtime_info=$( { time timeout $TIMEOUT_DURATION dotnet /code/output/main.dll < /code/testcases.txt > /code/output.txt; } 2>&1 )
        log_runtime_info $? "$runtime_info"
    else
        echo "BUILDFAILED" >> /code/error.txt
    fi

else
    echo "UNSUPPORTEDLANGUAGE" >> /code/error.txt
fi