pipeline {
    agent any

    environment {
        DOTNET_ROOT = '/opt/dotnet'
        PATH = "/opt/dotnet:$PATH"
    }

    stages {
        stage('Restore') {
            steps {
                sh 'dotnet restore'
            }
        }

        stage('Build') {
            steps {
                sh 'dotnet build --configuration Release'
            }
        }

        stage('Test') {
            steps {
                sh 'dotnet test'
            }
        }

        stage('Publish') {
            steps {
                sh 'dotnet publish -c Release -o ./publish'
            }
        }
    }
}
