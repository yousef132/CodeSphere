# Code Sphere

Code Sphere is an open-source online judge and contest platform built using various technologies, focusing on performance and security.

## Design and Architecture

- Utilizes clean architecture with separated layers to ensure a clear separation of concerns.
- Implements the Mediator Pattern, CQRS Pattern, Fluent Validation, and provides a unified response for all endpoints.

### 🧩 System Architecture

![Code Sphere Architecture](https://drive.google.com/uc?export=view&id=15SdlCAYca5hw87ddERx3jZE7wUA935O6)

This diagram represents the high-level structure of the system, showing interactions between components like frontend, backend services, judge engine, Docker, Redis, and Elasticsearch.
## Table of Contents

- [Design and Architecture](#design-and-architecture)
- [Technologies & Design](#technologies--design)
- [Features](#features)
- [Performance and Security](#performance-and-security)
- [Run Locally](#run-locally)
  - [Prerequisites](#prerequisites)
  - [Steps](#steps)
- [Errors and Solutions](#errors-and-solutions)
  - [Ensure LF Line Endings](#ensure-lf-line-endings)
  - [Configure Docker for Remote Access](#configure-docker-for-remote-access)

## Design and Architecture

- Utilizes clean architecture with separated layers to ensure a clear separation of concerns.
- Implements the Mediator Pattern, CQRS Pattern, Fluent Validation, and provides a unified response for all endpoints.

## Technologies & Design

- Docker & Docker Compose
- Redis
- Elasticsearch
- SignalR
- .NET 8
- JWT Token
- WebRTC

## Features

- Supports multiple programming languages such as Python, JavaScript, C++, C#, and Go.
- Offers peer programming through real-time meetings with a shared editor, allowing all participants to collaborate on the same code and submit solutions.
- Provides real-time code execution with results indicating statuses like TL, WA, RTE, TLE, and CE.
- Hosts timed contests for competition among users, complete with a live leaderboard.
- Includes plagiarism detection to identify cheating during contests.
- Tracks submitted code by maintaining a leaderboard and a history of submissions.
- Supports Markdown for problem descriptions.
- Communicates with users via an email service for notifications and email confirmations.

## Performance and Security

- Uses Docker to run code in a `sandbox` container, executing it against all problem test cases in an isolated environment with limited resources such as memory and time, returning the results.
- Implements `rate limiting`, as executing code requires creating a sandbox container and running all test cases, making it a costly process; thus, it is limited to 3 requests per minute.
- To handle the expected surge in requests during contests, problems are cached, and `Sorted Sets (ZSETs)` which is a Redis data structure used to store leaderboards, which suits our needs for contest standings.
- Since the primary search object is the problems, a robust searching strategy is essential; therefore, `Elasticsearch` is employed to enable high-performance and fuzzy searching with easy and fast filtering.

## Run Locally

### Prerequisites

- .NET 8
- Docker & Docker Compose
- Visual Studio
- SQL Server

### Steps

1. Clone the repository.
2. Navigate to the directory containing the docker-compose file and execute the command:
   ```sh
   docker compose up
   ```
   This will pull the necessary Docker images specified in the compose file (if they don't already exist) and start the services, including Redis, Elasticsearch, and Kibana, on your local machine.
3. Verify the database connection string for the SQL Server database.
4. pull compilers images for the language you need to try 
  * for example :
    *  `docker pull gcc:latest` for c++ 
    *  `python:3.8-slim` for python
6. Open the `Package Manager Console` and run:
   ```sh
   update-database
   ```
5. You are now ready to run the project.

## Errors and Solutions

### Ensure LF Line Endings

Ensure that the `run_code.sh` file uses `LF` line endings instead of `CRLF` for compatibility with Docker's default operating system (Linux).

### Configure Docker for Remote Access

1. Right-click on **This PC** and select **Properties**.
2. Navigate to **Advanced system settings**.
3. Click on **Environment Variables**.
4. Add a new variable:
   - **Variable Name:** `DOCKER_HOST`
   - **Variable Value:** `tcp://127.0.0.1:2375`
5. Open **Docker Desktop**.
6. In the bottom-right corner, right-click on the **Docker Desktop** icon.
7. Select **Settings**.
8. Enable the option: *Expose daemon on tcp://localhost:2375 without TLS*.

