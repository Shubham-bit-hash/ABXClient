# zicuro
---

# ABXClient

This project is a client that connects to the ABX server to request and manage packets. It communicates with a TCP server, retrieves packets, and handles the resend of missing sequences. It then serializes the data into a JSON file.

## Prerequisites

Before you begin, ensure that you have the following installed on your local machine:

1. **Node.js**: A JavaScript runtime to run the server-side code.
   - You can install Node.js from [here](https://nodejs.org/).

2. **.NET Core (if using C#)**: If you're running the C# code as part of the project.
   - You can download and install .NET Core from [here](https://dotnet.microsoft.com/download).

3. **Git**: To clone the repository.
   - You can download Git from [here](https://git-scm.com/).

4. **Visual Studio / Visual Studio Code** (optional, for editing and debugging code).

## Clone the Repository

First, clone this repository to your local machine:

```bash
git clone https://github.com/Shubham-bit-hash/ABXClient.git
cd ABXClient
```

## Setting Up the Node.js Server

To run the Node.js server locally, follow these steps:

### 1. Download the server

- You will find a zip file attached to this page named "abx_exchange_server".
- Download and unzip this file to access the contents.
- Enter the extracted folder.
- Run the command "node main.js" to start the ABX exchange server.

### 2. Start the Node.js Server

Run the following command to start the Node.js server:

```bash
node main.js
```

You should see a message like this:

```
TCP server started on port 3000.
```

This indicates that the server is running and listening for connections on port 3000.

### 3. Test the Node.js Server

Once the server is running, you can interact with it using the ABX client, which is explained below.


## Running the ABX Client (C# Code)

To run the ABX Client, follow these steps:

### 1. Install .NET Core

Make sure that you have **.NET Core SDK** installed on your local machine. If you don’t have it, you can download and install it from the [official .NET website](https://dotnet.microsoft.com/download).

### 2. Build the C# Project

After cloning the repository, navigate to the folder containing the C# project and restore the dependencies:

```bash
dotnet restore
```

### 3. Run the ABX Client

To run the client application, use the following command in the terminal or command prompt:

```bash
dotnet run
```

This will execute the C# program, connect to the ABX server, request packets, identify missing sequences, and save the data to a `output.json` file.


## Interacting Between the Node.js Server and C# Client

- The **Node.js server** listens for incoming connections and handles packet communication.
- The **C# client** connects to the Node.js server, requests data, and processes the received packets.

Make sure both the server (Node.js) and client (C#) are running simultaneously to enable communication.

## Troubleshooting

- **Port Issues**: If the Node.js server is unable to start, ensure that the port (3000) is not being used by another application. You can change the port in the `main.js` file if needed.
- **Missing Dependencies**: Ensure that all necessary dependencies are installed for both the Node.js and C# components. Run `npm install` for Node.js and `dotnet restore` for the C# application.



---
