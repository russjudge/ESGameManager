# ESGameManager

The purpose of this application is to provide a simplified mechanism for managing ROMS for Emulation Station.

I had obtained a small game box that included what was claimed to be over 100,000 games, which seemed pretty cool at the time.  However, as I started playing it, I noticed a
lot of duplicates, some games not working at all, and other issues.

In some cases, there was a long list of something like 10,000 games in a single folder (like the "Arcade" folder), and scrolling through the list to find the game I wanted to play
was a pain in the neck.

I discovered there is at least one application out there that helps with managing ROMS, but I found that it was way too limited for what I wanted to do, and coding for
it seemed pretty simple, so I created my own application.

The features of this application are all features I needed for my own use.  Upon completion I felt that this tool was useful enough for others, and it offered features not
available elsewhere, so I released it for others (TODO). 

# How it works

After setting the master ROM folder, all gameList.xml files in all subfolders are read and loaded into memory.  All fields can be edited.

In addition, added to the gameList.xml file are flags and note, which can be used for tracking issues and whatever you want.  These fields are ignored by Emulation Station,
but are used by ESGameManager for whatever purpose you desire.

Also, the ROMs in each folder can be reorganized into subfolders by first letter, genre, developer, publisher, or all in the main folder, which Emulation Station
will use for organizing the list of games.

The list of games can easily be sorted by id, file path, release year, or name by simply clicking on the column header.

You can also extract the entire list of games into a comma-delimited file for loading into a spreadsheet like Excel.

There is also a tool to scan the rom folder for missing or new rom files and orphaned media files.

ESGameManager, however, does not scrape for metadata in external databases.  This is a feature that is available within Emulation Station and other tools, so will not be
made available here.

ESGameManager is written in .NET 8.0, and as such should be able to be compiled in Linux.  However, the installer is only available for Windows.  A script for
compiling to Linux may become available at a later date.

## Installation

Currently only set to run on Windows.  To run on Linux, clone this repository and compile it in your Linux environment.

For Windows install, download and run the installer file from <https://github.com/russjudge/ESGameManager/Release/esgamemanagersetup.msi>

