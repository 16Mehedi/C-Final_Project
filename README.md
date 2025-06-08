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
``Initial solution structure created with projects and shared library setup``

---

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


## Day 4: Replaced Legacy Code & Improved Flexibility

### What Was Changed
- Replaced legacy `BinaryFormatter` with modern and safer `System.Text.Json` for serialization.
- Agents now accept command-line arguments:
  - First argument: directory path to scan (e.g. `./texts`)
  - Second argument: pipe name (e.g. `agent1`)
- Defaults are used if arguments are missing.
- Cleaned up code using `using` statements and simplified the threading model.
- Each `.txt` file scanned for words using regular expressions and lowercase normalization.

### Functional Verification
- Tested both agents with and without arguments.
- Confirmed data sent over named pipes to the master in JSON format.

### Commit Message




## Day 5: Final Integration, Testing & Documentation

### What Was Changed
- Set processor affinity in all apps:
  - Master → Core 0
  - AgentA → Core 1
  - AgentB → Core 2
- Master process waits for both agents, then prints all word counts per file.
- Resolved `.swp` Vim temporary file issue during Git commits.
- Manually created UML diagram 


### Final Testing
- Ran full test:
  - Master first
  - Then AgentA and AgentB
- Verified complete output with word counts from both directories.

### Commit Message


## How I Ran

1. **Start the Masster and run without debugging then it shows the output like "Waiting for AgentA"
2. Then i run AgentA  using another terminal and saw in masters terminal now it shows "Waiting for AgentA and AgentB"
3. then i run AgentB using another terminal and saw in master terminal it shows final output 
