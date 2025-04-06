pipeline {
    agent any

    tools {
        dotnet 'dotnet8' // cần cấu hình trong Jenkins global tool config
    }

    stages {
        stage('Clone') {
            steps {
                git 'https://github.com/devbaoo/SchoolManagement.git'
            }
        }

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

        // Tuỳ chọn: tạo artifact hoặc deploy
        // stage('Publish') {
        //     steps {
        //         sh 'dotnet publish -c Release -o ./publish'
        //     }
        // }
    }
}
