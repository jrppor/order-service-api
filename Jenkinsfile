pipeline {
  agent any

  environment {
    ECR_REPO = '514141761577.dkr.ecr.ap-southeast-1.amazonaws.com/order-service-api'
    AWS_REGION = 'ap-southeast-1'
    IMAGE_TAG = "${BUILD_NUMBER}"
    CLUSTER_NAME = 'dev-cluster'
    SERVICE_NAME = 'order-service-api'
  }
  

  stages {
    stage('Checkout') {
      steps {
        git branch: 'main',
            url: 'https://github.com/jrppor/order-service-api.git'
      }
    }

    stage('Restore & Build') {
      steps {
        sh 'dotnet restore OrderService.sln'
        sh 'dotnet publish Order.API/Order.API.csproj -c Release -o publish'
      }
    }

    stage('Docker Build & Push') {
      steps {
        sh "aws ecr get-login-password --region $AWS_REGION | docker login --username AWS --password-stdin $ECR_REPO"
        sh "docker build -t $ECR_REPO:$IMAGE_TAG -f Order.API/Dockerfile ."
        sh "docker tag $ECR_REPO:$IMAGE_TAG $ECR_REPO:latest"
        sh "docker push $ECR_REPO:$IMAGE_TAG"
        sh "docker push $ECR_REPO:latest"
      }
    }

    stage('Deploy to ECS') {
      steps {
        sh """
        aws ecs update-service \
          --cluster $CLUSTER_NAME \
          --service $SERVICE_NAME \
          --force-new-deployment \
          --region $AWS_REGION
        """
      }
    }
  }
}
