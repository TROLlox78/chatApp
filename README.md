# TCP Chat Application

This is a chat application prototype presented as a PUT (Telephone services programming) project. The application contains two solutions, the server and the client, and follows the MVVM architecture.

## Overview

The application opens port 666 for communication. It's compatible with .NET 6/8. and was written using Visual Studio 2022.

#### Server
The *server* starts a `TCPListener` and runs in an infinite while loop waiting for TCP handshakes from clients. When a new client establishes a session, the *server* creates a `Client` object. The `Client` object creates a `Task` (on a new thread) and begins running an infinite loop waiting for messages from the client from the opened network stream. In addition each `Client` gets saved to a `List` allowing for broadcasting of messages by the server.

#### Client
The *client* uses a `TCPClient` to connect to the server. Once a session is established a message with the username gets sent to the server and a new `Thread` listening to the server is open. Depending on the type of the message a different event is invoked and the GUI handles updating the state.


#### Protocol
The protocol designed for communication is binary. It uses a simple format where the first byte of any message represents it's _type_, next a 4 byte integer represents the length, and finally the rest is 7-bit ASCII characters.
| Opcode  | Length  |  Message|
|-------|-----|-------|
| 1-byte | 4-bytes | variable |

- Opcode 0 - Client->Server. Contains the username. Used at the start of a session.
- Opcode 1 - Server->Clients. Contains the username. Notifies clients of a new user.
- Opcode 5 - Server<->Client. Contains the message. Used to send messages to the server and to transmit messages to the clients.
- Opcode 10 - Server->Clients. Notifies that a user has disconnected.

The client does not send a disconnect message, instead the server knows to transmit a disconnect to the other clients when the TCP session ends.


#### WPF
The graphical interface was created in WPF, which is a microsoft gui framework, I have not programmed in this framework since and do not remember many details, the MVVM architecture was pretty confusing to use and I probably won't use it in future projects.

## Installation

- Install dotnet-sdk
- Clone the repository
```bash
$ git clone https://github.com/TROLlox78/chatApp
```
You will see two directories, chatApp and chatServer, go into each solution and launch it
- Launch the server
```bash
$ cd chatServer
$ dotnet run
```
The server is going to ask you for the IP. Check it with `ipconfig` or `ip a`.

- Launch the client
```bash
$ cd chatApp
$ dotnet run
```
Unfortunately the client uses WPF which is a Windows exclusive GUI 😫. Therefore the client can only run on Windows computers.


## Usage
Launch as many clients as you please and connect by typing in a name and the IP of the server. If you port forward port 666 you can even connect over the internet!
<img width="1519" height="960" alt="image" src="https://github.com/user-attachments/assets/206c9d26-fab6-42e0-996d-26b4b28771bc" />


