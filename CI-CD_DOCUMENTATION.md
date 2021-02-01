# Bogus CI/CD documentation

The pipeline:

- contains the Build and Test processes of your project
- uploads the results of the Test process as a pipeline artifact 
- executes on push commit, when a PR is created or merged and can also be executed manually

## 1. Build process

## 2. Test process

## 3. Test results artifact

## 4. Run workflow manually

  1. Go to GitHub project -> "Actions" tab

  2. From the "Workflows" list on the left, click on "Bogus CI/CD Pipeline"

  3. On the right, next to the "This workflow has a workflow_dispatch event trigger" label, click on the "Run workflow" dropdown, make sure the **master** branch is selected in the "Use workflow from" dropdown and click the "Run workflow" button

![Actions_workflow_dispatch](/Docs/CI-CD_DOCUMENTATION/Actions_workflow_dispatch.png)

  4. Once the workflow run has completed successfully, move on to the next step of the documentation

Built with ❤ by [Pipeline Foundation](https://pipeline.foundation)