# Build Your Own Binary
This is a Pattern Drawing Game, wherein User is supposed to recreate the pattern using a set of line directions which are represented in binary.

## Overview

The game is divided into 2 Main Parts - 
1. User has to select the starting point on the grid from where drawing can be started.
2. Using the Line Directions Panel on the left user has to draw the pattern

## How To Play
There is Windows File Executable in the Build Folder. You will have to download the folder and just the run the game to play!

## Step 1: Start Point Selection
Below is the Binary Table that Represents what binary value that corresponds to row and column number of the cell on the grid

| Binary | Decimal | Row        | Column |
|:------:|:-------:|:--------------:|:----------:|
| 0000   | 0       | 1          | 1       |
| 0001   | 1       | 1          | 2       |
| 0010   | 2       | 1          | 3       |
| 0011   | 3       | 1          | 4       |
| 0100   | 4       | 2          | 1       |
| 0101   | 5       | 2          | 2       |
| 0110   | 6       | 2          | 3       |
| 0111   | 7       | 2          | 4       |
| 1000   | 8       | 3          | 1       |
| 1001   | 9       | 3          | 2       |
| 1010   | 10      | 3          | 3       |
| 1011   | 11      | 3          | 4       |
| 1100   | 12      | 4          | 1       |
| 1101   | 13      | 4          | 2       |
| 1110   | 14      | 4          | 3       |
| 1111   | 15      | 4          | 4       |


## Step 2: Line Direction Selection
Below is the Binary Table That Represents in which direction you can extend the line in based on the binary number entered.

![Screenshot 2025-01-28 213320](https://github.com/user-attachments/assets/d4d9aab7-5db3-4c69-8bb7-a84815dc5636)

## Challenge:
Try to complete all the shapes with lowest Moves possible.
