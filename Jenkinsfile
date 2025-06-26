pipeline {
  agent any

  environment {
    ECR_REPO     = '514141761577.dkr.ecr.ap-southeast-1.amazonaws.com/order-service-api'
    AWS_REGION   = 'ap-southeast-1'
    IMAGE_TAG    = "${BUILD_NUMBER}"
    CLUSTER_NAME = 'dev-cluster'
    SERVICE_NAME = 'order-service-api'
  }

  stages {

    stage('Checkout') {
      steps {
        checkout scm
      }
    }

    stage('Docker Login') {
      steps {
        withCredentials([usernamePassword(credentialsId: 'jenkins-ecr', usernameVariable: 'AWS_ACCESS_KEY_ID', passwordVariable: 'AWS_SECRET_ACCESS_KEY')]) {
          sh '''
            aws configure set aws_access_key_id $AWS_ACCESS_KEY_ID
            aws configure set aws_secret_access_key $AWS_SECRET_ACCESS_KEY
            aws configure set default.region $AWS_REGION
            aws ecr get-login-password --region $AWS_REGION | docker login --username AWS --password-stdin $ECR_REPO
          '''
        }
      }
    }

    stage('SonarQube Scan') {
      steps {
        withSonarQubeEnv('SonarQube') {
          sh '''
            dotnet tool install --global dotnet-sonarscanner
            export PATH="$PATH:/root/.dotnet/tools"
            dotnet sonarscanner begin /k:"order-service-api"
            dotnet build OrderService.sln
            dotnet sonarscanner end
          '''
        }
      }
    }

    stage('Docker Build & Push') {
      steps {
        sh '''
          docker build -t $ECR_REPO:$IMAGE_TAG -f Order.API/Dockerfile .
          docker tag $ECR_REPO:$IMAGE_TAG $ECR_REPO:latest
          docker push $ECR_REPO:$IMAGE_TAG
          docker push $ECR_REPO:latest
        '''
      }
    }

    stage('Deploy to ECS') {
      steps {
        sh '''
          aws ecs update-service \
            --cluster $CLUSTER_NAME \
            --service $SERVICE_NAME \
            --force-new-deployment \
            --region $AWS_REGION
        '''
      }
    }
  }
}
