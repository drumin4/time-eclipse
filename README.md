
# Time Eclipse

The purpose of this program is to help people stay productive and to keep them aware of how much time they spend daily on their most important activities.

Time Eclipse is strictly a windows-only program and it is meant to be left running in the background while activities are undergoing. 


## How does it work?

### The program is based on individual accounts, which are stored in a database built in MSSQL. Therefore, the first step is to create an account, and then login. 
(the Username and Password have a few requirements that need to be satisfied in order to complete the registration)
#

![App Screenshot](https://i.postimg.cc/Xq16hzXM/loginSs.png)



#### After logging in, every new user will be asked to go through a guide to understand how TimeEclipse works. Although it is recommended, the guide is not mandatory and it will stay accesible at any time, having a separate tab dedicated to it.

#### The next tab after the Guide one is "Add Activity". Here, the user will be required to CREATE the activity first (on the pink, left side). A representative name and picture will be needed to complete this process. 



![App Screenshot](https://i.postimg.cc/wvDsznmm/Page-8.png)

#### Then, in order to add the created activity for further use (on the right, cream-colored side this time), it must be added manually from the drop box. A target time must also be specified. Based on it, the program will be able to monitor the performance and send feedback.
(H - Hours; M - Minutes; S - Seconds)

![App Screenshot](https://i.postimg.cc/LXFnxvb6/Page-9.png)
#

#### Once created and added, the activity will be ready to use in the Tracker page, which is accessible through the Tracker tab.

#### Here, the first step is simple. On the upper-left side of the page, the full name will be shown. Right below it, there is a drop bar. This can be used to choose the desidered activity, in order to start tracking it. By doing this, the activity will be added to the table below. 
(It is allowed to add multiple activities to the table, but only one can be tracked at a time. This design was chosen in order to help the user avoid multi-tasking, as it has been proven to be quite an inefficient habit)

#### In the table, every activity will be shown with a few specifications regarding each one of them:
- Activity ID - it is a unique number that's automatically generated; 
- Activity Name - it's the name of the activity given by the user;
- Date Started - the date when the activity is added to the table is considered the date when it started being tracked. Naturally, this information will not change over time;
- Current Date - it's the date when the user tracks an activity and later on saves it as a record;
- Time Spent - the amount of time that has been dedicated to the current activities. This updates every time one of the Pause/Reset/Stop commands is pressed for the stopwatch;
- Target Time - the amount of time that has been set as the target. If the Time Spent is higher than the Target Time, it means the user has completed the activity and can stop it at any time;
- Status - depending on the user's actions, the status can be one of the following: Unstarted/Ongoing/Paused/Stopped. To be noted that the Reset button automatically sets the status to Unstarted.

![App Screenshot](https://i.postimg.cc/fW1Dm5JV/Page-11.png)

#### After adding all the activities wanted to track in the table, the user can select one of them by clicking on its own row (double-clicking will result in the REMOVAL of the activity from the table). 
#### Now, the stopwatch that's located on the upper-right side of the page will focus its commands on the selected activity. The 4 commands available are:
- Start - when the user begins to engage in the selected activity, the stopwatch should be started in order to time the amount of time spent;
- Pause - in case a break from the activity is needed, or if the user wants to switch to another activity and come back to the previous one later, the Pause button will place the activity on hold;
- Reset - it simply sets to default the selected activity. It means that the Time Spent on that activity will be set to 0;
- Stop - this button is to be pressed ONLY when the activity is thoroughly finished for the day. After stopping an activity, it will NOT accept any further modifications.
- ADDITIONAL COMMAND: the Finish button is located at the right side of the table. When pressed, no matter the status of the activities (Unstarted/Ongoing/Paused/Stopped), all of them will be considered finished and will be saved as records with no chance of further modifications. This button is mostly reserved to be used at the end of the day, when no further engagement is planned to be taken anymore.

![App Screenshot](https://i.postimg.cc/DyVDgNFb/Page-12.png)
#

#### When the Finish button is pressed, the program automatically redirects the user to the Records Page. There is also a specific tab that the user can access to check the records at any time. 

#### On the record page, there will be a text box available on the upper-left side, which the user can use to look up a certain activity by its name.
#### Next to the text box, on its right, there is a table, which lists all saved records. It specifies 4 elements for each record:
- Date Performed - the date when the activity was saved as a record;
- Record Name - it is the same as the activity name given by the user;
- Date Started - as stated above, it's the date when the activity is tracked for the first time;
- Performance - the format of this element is composed of Time Spent / Target Time. It practically shows how much time was dedicated to the activity in comparison to how much time was targeted for it.
#### Clicking on any record on the table will immediately cause the chart below to show the performance of the last 5 records (for the activity selected). It is here where the feedback is given, which should serve as an encouragement to either perform better or to just stay on track.

![App Screenshot](https://i.postimg.cc/Yq1xK1yv/Page-16.png)

#### Last but not least, to exit TimeEclipse, you first have to sign out, which you can do by simply pressing the Sign Out button on the upper-right side of the screen.
## FAQ

#### What inspired you to build TimeEclipse?

Upon trying to think of what kind of personal project I could build for my portfolio, I thought the best idea would be to create something practical, that has some sort of utility in real life. Eventually, the idea of productivity came to mind. So I have designed this program with the goal of making it as engaging and beginner-friendly as possible. What inspired me the most is the fact that I myself wanted to use this program for my own purposes. So I was constantly flooded with ideas of what features I would like to add to make my life easier. Overall, TimeEclipse as been a fun ride that further deepened my passion for programming and its endless possibilities.

#### How was the process of building TimeEclipse like?

After building up my programming skills for quite an extended period of time, I finally decided to put my knowledge to the test. TimeEclipse was the challenge I needed, but the process wasn't as smooth as I had anticipated. 

From drawing the UI design, to building up the concepts and functionalities that I had in my mind, I was constantly met with challenges and roadblocks. Overcoming them wasn't always easy, but the constant success was undoubtly building up my confidence and general knowledge as a software developer. 

As I kept moving forward, what I wanted my program to be had become much clearer. From a certain point, things made more sense and new ideas started to come up.

#### What would you add/change about your program if you were to do it all over again?

I realized this too late, but I would have liked it if I had created classes of objects for every table in my database (such as User, Activity and so on). By not doing it, I was unable to undergo unit tests for my program, since I based it around my database entirely.

When it comes to additional features, I have a few in mind, such as implementing a Dark Mode feature, or a Minimize Window option (since TimeEclipse was designed to run in the background most of the time).

Some of the best realizations I have achieved are: 
- 
- I developed a more analytical mindset. I also learned to ask the right questions and search for the right information to solve unexpected problems and move forward.
- I have been stuck numerous times during the project. As discouraging as it sometimes was, I did learn that there is always a solution for any problem. Patience and taking things step by step goes a long way.
- I have deepened my understanding around concepts that I wasn't feeling that comfortable with, such as: 
1) How databases work and how to manipulate data according to your needs;
2) How to work with .Net Core and use NuGet Packages (I eventually had to switch the whole project to .Net Framework, since .Net Core imposed some limits with UI, causing bugs)
3) Working with value types such as DateTime and TimeSpan;
4) Working with collections such as Dictionaries and Lists;
5) How to use Windows Forms in a way that makes the UI more engaging;
6) Applying CRUD and KISS principles;
7) Testing my code and identifying what wasn't working.



## 🚀 About Me
I'm a young, ambitious programmer, looking forward to getting out of my comfort zone and achieving great things along other inspiring programmers. 

