# CodeProjectSamples
A collection of code samples and projects.

## Project Descriptions

### _Bank Account Final Project, First Year:_
	
This is a very simple command line project done in Fall 2017 for an Intro to Programming course. It is a type of application that a bank teller would use to add, edit, remove, and check up on client accounts. This project is included here because it is my starting point in my programming journey.

To run the project, you will require an ID and a Password (located in _data\tellers.dat_), but for examples purposes use:

User: o-fritz

Pass: happy

### _Endless Runner Game, First Year:_
	
This is a Unity game built for my Game Technology I course in Spring 2018. For the final project, we were given 2 weeks to concept and develop a basic prototype of a 2D game. I set myself a massive challenge in that I wanted to learn and implement some of Unity's built-in Networking libraries and assets. I learned and roughly implemented UNET libraries and assets to create a networked game that can be successfully played using P2P methods. The majority of the art assets were also created by me, with the exception of a few textures, since I've taken a 3D modeling course.

After project completion, I left it as it was. I am extremely proud of what I was able to get done in such a short amount of time, and this is the result exactly as I left it. The game itself is located in _Running Project\Test3.exe_. The source code is located in _Source Project\Assets\Scripts_.

### _Apriori Final Project, Second Year:_

This project was for Data Structures and Algorithms, completed in Fall 2018. The goal is to run the Apriori mining association algorithm across an IBM-generated dataset. This is an unorthodox project for second-year Data Structures students, and a very difficult one to complete. It was a partner project, and we were successfully able to get Apriori to run on small to mid-size datasets. We do have a known issue where memory management becomes problematic with datasets exceeding 100,000 entries, and any dataset larger than 1,000 takes a long time to complete.

For this reason, the _Running Project_ directory only runs Apriori over a very small dataset to save time and show that it works.

### _Sorting Algorithms, Second Year:_

I've included this as a simple demonstration to show that I can, in fact, implement various sorting algorithms succcessfully.

### _Inkantation Game, Second Year:_

This game is very interesting, completed for Production I in Spring 2019 as a 6 week final project. It was a team project, with a team composed of 1 artist, 1 producer, 2 designers, and 2 programmers. Unfortunately, it will not be playable as it _requires_ a DolphinBar USB sensor bar, a Wii Remote, and a Wii Nunchuk. The goal of this game was to use a controller other than mouse and keyboard. My team opted for the Wii controller as it is very useful when it comes to having users draw symbols on-screen, which is the premise of the game. In this game, the player must navigate and fight through an authoritarian city as a rebel graffiti artist with the power to summon demons with his graffiti. The player may summon these demons by using the Wii remote to draw certain symbols on the screen. If the symbol is accurate enough, the demon will successfully spawn and begin its actions, or grant certain buffs.

The folder _InkantationGame\Source Code\Gesture Recognizer Scripts_ contains the essential code for accurately recognizing gestures. This library was found on the Unity Asset store, _however_ it was only meant to function in 2 dimensions as a drawing game such as Drawful or Skribbl.io. We needed to modify the mathematics and data structures to function in 3D, which is why it is included as source code since I was able to modify the library to our specific needs in a very efficient way that saved us a lot of time. Credit for the original library is given in our technical plan documentation. All Gameplay Scripts were entirely created by the programmers.

This is by-far one of the most unique projects I've ever gotten to take part in, and I am very proud of the results we were able to get from it, regardless of the visual status and bugs.

There are two videos to demonstrate basic gameplay, one showing off mechanics [here] (https://drive.google.com/file/d/1Lmtsg-QFO_3-PcRfpXb9nU9Kpf5VyCPw/view?usp=sharing), and the other being a mock trailer put together by our producer [here] (https://drive.google.com/file/d/1ZlAAhVon_lzwQjA0JZdBeSkfF00nxgtI/view?usp=sharing).