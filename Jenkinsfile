pipeline {
    agent any

    tools {
        customTool 'dotnet8'
    }

    stages {
        stage('Restore') {
            steps {
                sh '$DOTNET8_HOME/dotnet restore'
            }
        }

        stage('Build') {
            steps {
                sh '$DOTNET8_HOME/dotnet build --configuration Release'
            }
        }

        stage('Test') {
            steps {
                sh '$DOTNET8_HOME/dotnet test'
            }
        }

        stage('Publish') {
            steps {
                sh '$DOTNET8_HOME/dotnet publish -c Release -o ./publish'
            }
        }
    }
}
