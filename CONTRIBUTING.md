# How-to contribute
Are you missing a feature or just want to help out? To maximize the chances of getting your pull-request accepted, please take a moment to read this guide on how to contribute.

## Setting things up 

**1)** Fork the project and add an upstream. [More info](https://help.github.com/articles/fork-a-repo).

**2)** Create a `feature` branch from the `develop` branch: `git checkout -b my-feature-branch develop`

**3)** In the root there's a `setup-devenv.bat` file which will use Ruby or more precisely, `Rake` to download and install NuGets. **Note!** Since NuGet v2.0.0, you need to allow NuGet to do package restore. This is a one time setting per computer. You can _edit this in Visual Studio_ under the menu: `Tools --> Library package manager --> Package Manager settings --> Package restore --> Allow NuGet to download...`

**4)** Tests are written using `NUnit`.

## Work, work, work...
* Please, try to keep your changes small and focused. Small commits are preferred.
* Write proper commit messages! Commit messages should be helpful to others.
* Do your work in the open. Share your ideas with the community as early as possible.
* I'm a windows guy. I want my CRLFs. Personally, I'm using `core.autocrlf=false`.

## Before sending the pull request

**1)** Pull in changes to your `develop` branch from the official repository.

```
git checkout develop
git pull upstream/develop
```

**2)** Rebase your feature branch against develop.

```
git checkout my-feature-branch
git rebase develop
```

**3)** Finally, merge in you feature branch to `develop`
```
git checkout develop
git merge --no-ff my-feature-branch
```

## Pull request
Now you can do the pull request against the official develop branch. [Read more about pull-requests here.](https://help.github.com/articles/using-pull-requests)