https://docs.github.com/en/get-started/importing-your-projects-to-github/importing-source-code-to-github/adding-locally-hosted-code-to-github

git init .
create .gitignore file
git add .
git commit -m "First commit"
git remote add origin https://github.com/jordiascension/azure-functions.git
git remote -v
git push origin master (or git push origin main)

-add new changes
git add .
git commit -m "Added .gitignore changes"
git push origin master

-branches
git checkout -b feature/update-readme
git add .
git commit -m "Added .gitignore changes"
git push origin feature/update-readme


