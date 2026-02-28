# 🌱 AgroSolutions.Property
> O serviço de gestão de propriedades AgroSolutions Hackathon mantém o contexto das propriedades rurais, juntamente com seus terrenos e as culturas ali presentes..

## 🚜 Funcionalidades
  - Busca de todas as Culturas cadastradas;
  - Busca de todas as Propriedades do Produtor Rural;
  - Busca de uma Propriedade do Produtor Rural pelo seu identificado;
  - Cadastro de Propriedade para o Produtor Rural;
  - Atualização cadastral da Propriedade do Produtor Rural;
  - Exclusão da Propriedade do Produtor Rural;
  - Cadastro de Talhões para uma Propriedade do Produtor Rural;
  - Atualização cadastral do Talhão de uma Propriedade do Produtor Rural;
  - Exclusão do Talhão de uma Propriedade do Produtor Rural;
  - Exclusão de toddos os Talhões e de todas as Propriedades do Produtor Rural dado a sua exclusão no [Microsserviço de Identidade](https://github.com/Hackaton-AgroSolutions/AgroSolutions.Identity);

## ⚙️ Requisitos não funcionais
  - O sistema garante a segurança via autenticação, autorização e Instrospecção do Token Microsserviço de Identidade com JWT.
  - O sistema garante a integridade dos dados com validações internos à base de dados.
  - O sistema suporta escalabilidade horizontal conforme aumento de carga com HPA.
  - O sistema garante confiabilidade e consistência eventual na comunicação orientada a eventos.
  - O sistema garante manutenabilidade dado os microsserviços desacoplados.
  - O sistema prove observabilidade, com métricas, logs e logs distribuídos rastreáveis.
  - O sistema garante atualizações contínuas do artefeto de produção com fluxos de integração e entrega contínua.

## 🏗️ Desenho da Arquitetura
<img width="4123" height="4559" alt="Diagrama" src="https://github.com/user-attachments/assets/71eff8d1-67e7-42bf-9005-11694eaa4f83" />

## 🛠️ Detalhes Técnicos
### ⭐ Arquitetura e Padrões
 - Arquitetura orientada a eventos (Event-Driven Architecture – EDA);
 - Notification Pattern (Exceptionless);
 - Padrão CQRS (Command Query Responsibility Segregation);
 - Mediator Pattern com MediatR;
 - Clean Architecture;
 - Unit of Work;
 - Arquitetura baseada em APIs REST;
 - Uso de Middlewares e Action Filters para cross-cutting concerns;
 - Uso de CustomAttribute para realização da _Introspecção de token_(Validar a existência do Usuário no Microsserviço de Identidade);
 - Microsserviços containerizados.

### ⚙️ Backend & Framework
 - .NET 10 com C# 14;
 - ASP.NET Core;
 - Entity Framework Core;
 - FluentValidation para validações robustas;
 - BackgroundService;
 - Autenticação e autorização via JWT;
 - Documentação de APIs com Swagger / OpenAPI.

### 🗄️ Banco de Dados & Mensageria
 - SQL Server;
 - RabbitMQ para mensageria assíncrona;
 - Comunicação orientada a eventos;
 - Logs distribuídos com CorrelationId para rastreabilidade.

### 📊 Observabilidade & Monitoramento
 - Prometheus para coleta de métricas;
 - Grafana Loki para centralização de logs;
 - Estratégia de logging estruturado e distribuído.

### 🧪 Testes
 - Testes unitários com xUnit;
 - FluentAssertions para assertions mais expressivas;
 - Moq para criação de mocks e isolamento de dependências.

### 🚀 DevOps & Infraestrutura
 - CI/CD self-hosted;
 - Docker para containerização;
 - Kubernetes (Deployments, Services, HPA, ConfigMaps e Secrets);
 - Kong API Gateway para gerenciamento e roteamento de APIs.

## 🧪 Testes
  - Navegue até o diretório dos testes:
  ```
  cd ./AgroSolutions.Property.Tests/
  ```
  - E insira o comando de execução de testes:
  ```
  dotnet test
  ```

## ▶️ Execução
  - Via HTTP.sys:
    - Navegue até o diretório da camada API da aplicação:
    ```
    cd ./AgroSolutions.Property.API/
    ```
    - Insira o comando de execução do projeto:
    ```
    dotnet run --launch-profile https
    ```
    - Acesse [https://localhost:7075/swagger/index.html](https://localhost:7075/swagger/index.html)

  - Via Kubernertes local (minikube/kind):
    - Execute o comando para aplicar todos os arquivos yamls presentes no diretório:
    ```
    kubectl apply -f .\k8s\    
    ```
    - Em seguida faça o PortForward:
    ```
    kubectl port-forward svc/agrosolutions-property-api 8081:80
    ```
    - Acesse [http://localhost:8081/swagger/index.html](http://localhost:8081/swagger/index.html)

## 🚀 Requisições para Kong API Gateway
```javascript
await fetch("/property/api/v1/crops", { method: "GET" })

response = await fetch("/identity/api/v1/auth/login", {
  method: "POST",
  body: JSON.stringify({ email: "demo@gmail.com",  password: "password1234$$" }),
    headers: {
    ...headers,
    Authorization: `Bearer ${token}`
  }
}).then(r => r.json());
token = response.data.token;
const headers = {
  "Content-Type": "application/json",
  Authorization: `Bearer ${token}`
};

// Create property
response = await fetch("/property/api/v1/properties", {
  method: "POST",
  body: JSON.stringify({
    name: "New Property",
    description: "New Property Description"
  }),
  headers
}).then(r => r.json());
const propertyId = response.data.propertyId;
response = await fetch("/property/api/v1/properties", {
  method: "POST",
  body: JSON.stringify({
    name: "New Property 2",
    description: "New Property Description 2"
  }),
  headers
}).then(r => r.json());
const propertyIdToDelete = response.data.propertyId;

// Get Properties from user
await fetch("/property/api/v1/properties", {
  method: "GET",
  headers
});

// Get Properties by Id from user
await fetch(`/property/api/v1/properties/${propertyId}`, {
  method: "GET",
  headers
});
await fetch(`/property/api/v1/properties/${propertyIdToDelete}`, {
  method: "GET",
  headers
});

// Delete Property by Id from user
await fetch(`/property/api/v1/properties/${propertyIdToDelete}`, {
  method: "DELETE",
  headers
});

// Update property
await fetch(`/property/api/v1/properties/${propertyId}`, {
  method: "PATCH",
  body: JSON.stringify({
    name: "New Field",
    description: "New Field Description"
  }),
  headers
});

// Create property fields
response = await fetch(`/property/api/v1/properties/${propertyId}/fields`, {
  method: "POST",
  body: JSON.stringify([
    {
      cropId: 1,
      name: "Field number 1",
      totalAreaInHectares: 12
    },
    {
      cropId: 2,
      name: "Field number 2",
      totalAreaInHectares: 8
    }
  ]),
  headers
}).then(r => r.json());
const fieldId = response.data[0].fieldId;
const fieldIdToDelete = response.data[1].fieldId;

// Update property field
await fetch(`/property/api/v1/properties/${propertyId}/fields/${fieldId}`, {
  method: "PATCH",
  body: JSON.stringify({
    cropId: 3,
    name: "New Field number 1",
    totalAreaInHectares: 16
  }),
  headers
});

// Delete property field
await fetch(`/property/api/v1/properties/${propertyId}/fields/${fieldId}`, {
  method: "DELETE",
  headers
});
```
