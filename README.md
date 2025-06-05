
# Distributed Word Indexing System

## Day 1: Project Initialization

### Objective
- Set up solution structure with four projects:
  - AgentA
  - AgentB
  - Master
  - SharedLib

### Setup Steps
- Created a new Visual Studio solution `DistributedWordIndexing`.
- Added `AgentA`, `AgentB`, and `Master` as C# Console Applications.
- Added `SharedLib` as a Class Library project.
- Defined `WordIndex` class in `SharedLib` for data exchange.
- Added project reference to `SharedLib` from all three applications.
- Verified build and startup of each console project with placeholder output.

### Commit Message


## Day 2: Implement Agent File Scanning & Word Indexing

### What Was Done
- Implemented file scanning and word counting in `AgentA` and `AgentB`.
- Word counts are stored in `WordIndex` class.
- Applied `ProcessorAffinity` to assign CPU core for each agent.
- Used `Thread` to perform file scanning.

### Functional Verification
- Added `.txt` files in `textsA/` and `textsB/` directories.
- Verified word count results printed by each agent.

### Commit Message

## Day 3: Implement Inter-Process Communication (Named Pipes)

### What Was Done
- Agents:
  - Scanned `.txt` files and serialized `WordIndex` lists.
  - Sent data using `NamedPipeClientStream` to the Master.
- Master:
  - Used `NamedPipeServerStream` to receive data from both `AgentA` and `AgentB`.
  - Used `Thread` to listen on both pipes concurrently.
  - Deserialized and stored results in a shared list.
- Used `BinaryFormatter` for serialization (with suppression of warnings).
- Output confirmed both agents successfully sent data.

### Functional Verification
- Started Master process first.
- Then ran AgentA and AgentB.
- Verified that Master received and printed word counts from both agents.

### Commit Message

