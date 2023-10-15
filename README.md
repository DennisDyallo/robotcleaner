# robotcleaner

# Background

When you have a lot of people working in an office it can get dirty quite quickly if you’re not careful. However, cleaning staff are expensive. To save money on cleaning staff the best solution was deemed to be the creation of an automatic cleaning robot that cleans the office at night.

# Assignment

Your assignment is to build a prototype of this robot’s scheduling software. Since it’s only a prototype, the scope and required functionality is limited, and allows for some flexibility and interpretation in its implementation. However just because it’s simple doesn’t mean it’s not important, and it’s expected to be built to be both easy to maintain and easy to expand upon in the future.

# Requirements

The robot should use a Cartesian coordinate system to understand its position.

At start, the robot should take in its map, its starting position, and its intended route according to the following format standard:

M:MinX,MaxX,MinY,MaxY;S:StartingX,StartingY;[Direction+Length]

Example: M:-10,10,-10,10;S:-5,5;[W5,E5,N4,E3,S2,W1]

The input has 3 sections, separated by semicolons - the order is not important.

M stands for Map, and takes in 4 values representing the limits of the Cartesian coordinate system.

S stands for Start, and takes in a coordinate point, which is where the cleaner will start form.

Between [ and ] will be an array of values representing Direction and Length, separated by commas.

Directions are the representations of the 4 cardinal points: N, E, S, and W

Length is how far the robot should travel in that direction

The format is Direction+Length, like “N10”, “E2”, “S4”

Once the robot starts, it shouldn’t stop until it has completed its planned route, unless it goes outside of its Map. If this happens, an error should be displayed showing its current position and why it can’t execute its path, along with all positions it has traveled so far.

At the conclusion of the robot’s route (if successful), it should output all the unique coordinates it has visited (and hopefully cleaned).

Bad input and errors should be handled with grace, and should provide helpful feedback.

There is no need for an advanced UI; a simple command line is sufficient.



# Examples:

Here are some examples of input/output:

Using Input: M:-10,10,-10,10;S:-5,5;[W5,E5,N4,E3,S2,W1]

All Positions Cleaned:

-5,5;-6,5;-7,5;-8,5;-9,5;-10,5;-9,5;-8,5;-7,5;-6,5;-5,5;-5,6;-5,7;-5,8;-5,9;-4,9;-3,9;-2,9;-2,8;-2,7;-3,7

Unique Positions Cleaned:

-5,5;-6,5;-7,5;-8,5;-9,5;-10,5;-5,6;-5,7;-5,8;-5,9;-4,9;-3,9;-2,9;-2,8;-2,7;-3,7

-----------------------

Using Input: [W1,N1,E1,E1,S1,S1,W1,W1,N1,E1];S:0,0;M:-1,1,-1,1

All Positions Cleaned:

0,0;-1,0;-1,1;0,1;1,1;1,0;1,-1;0,-1;-1,-1;-1,0;0,0

Unique Positions Cleaned:

0,0;-1,0;-1,1;0,1;1,1;1,0;1,-1;0,-1;-1,-1

-----------------------

Using Input: S:0,0;[N1,N1,N1,N1,N1,S1,S1,S1,S1,S1];M:0,0,0,5

All Positions Cleaned:

0,0;0,1;0,2;0,3;0,4;0,5;0,4;0,3;0,2;0,1;0,0

Unique Positions Cleaned:

0,0;0,1;0,2;0,3;0,4;0,5
