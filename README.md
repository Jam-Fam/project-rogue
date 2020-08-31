# Project Rogue

[Trello Board](https://trello.com/b/aKf2KCnF/jamfam-community-game)

## Technology
[Unity 2019.3.0f3](https://unity3d.com/unity/beta/2019.3.0f3)

## How To Contribute

### 1. Sign Out an Objective 
Pick an Objective card from the To Do List on the [trello board](https://trello.com/b/aKf2KCnF/jamfam-community-game). Transfer that card into the Taken Task column, then add yourself to the Objective by clicking on the card, then on Members (under "Add to card"), and then selecting yourself.

### 2. Create a Git Branch
#### In Git Bash:
Create and checkout a new branch with the command `git checkout -b `. Name the branch with one of the following styles, depending on the "Type" field on your Objective card: 
`feature/branch-name`
`bugfix/branch-name`
`rework/branch-name`

### 3. Make Your Changes
Work as normal, writing and testing your code in Unity. Try to keep any changes within the scope of the Objective card(s) you have signed out.

### 3. Commit and Push Your Changes
#### In Git Bash:
Run the following commands in order:

 * `git status` - This lets you review any changes you've made. Check this out to make sure you're not about to push changes you don't want to.
 * `git add .` - 'Add' stages your files and prepares to commit them. 
Note: Using the `.` will stage all your changed files. If you don't want to do that, you can also add files individually with the following command:
 `git add file/name/here.cs`
 * `git commit -m "Describe your changes here.` This will commit the changes you've made so that you can push them to the public repo.
 * `git push origin insert/branch-name` Replace 'insert/branch-name' with the name of your branch. E.g. `git push origin feature/basic-movement`

### 4. Create a Pull Request
Now your changes will be pushed to GitHub, but will not yet be merged with the master branch. When you're finished and think you're ready to merge your code with the master branch, you can submit a Pull Request. On the repository [main page](https://github.com/Jam-Fam/project-rogue), switch from the 'master' branch to the one you just created.
