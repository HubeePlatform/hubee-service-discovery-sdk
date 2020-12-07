# Hubee Service Discovery Sdk

![N|Solid](https://media-exp1.licdn.com/dms/image/C4E0BAQHOp41isf2byw/company-logo_200_200/0?e=1611792000&v=beta&t=R627Tkw1cwQgb-LjNTJh_4auJWQsQieuU4wHoyLfIDA)

Hubee Service Discovery Sdk é uma biblioteca que faz abstração da implementação de service discovery nas aplicações. A principal ideia desse SDK é abstrair toda a complexidade das configurações e ser adaptável para as mudanças de tecnologias, centralizando toda manutenção e evolução em um único ponto.

## Service discovery implementados

- Consul

## Getting started

Após realizar a instalação do SDK em seu projeto podemos iniciar a configuração para utilizá-lo, segue abaixo a configuração que deve ser realizada na seção **"HubeeServiceDiscoveryConfig"** dentro do arquivo appsettings:

```json
  "HubeeServiceDiscoveryConfig": {
    "ServiceName": "Nome da aplicação",
    "ServiceDiscovery": "Consul",
    "HostName": "localhost",
    "Port": "8500",
    "HealthCheck": {
      "Endpoint": "healthcheck",
      "Interval": 60,
      "DeregisterCriticalServiceAfter": 60
    }
  }
```

| Configuração | Observação |
|:----|:----------|
| ServiceName | nome do serviço |
| ServiceDiscovery | service discovery que será utilizado |
| HostName | host do service discovery |
| Port | porta do service discovery |
| HealthCheck.Endpoint | endpoint de health check do serviço |
| HealthCheck.Interval | valor em segundos que especifica a frequência de execução da verificação do health check |
| HealthCheck.DeregisterCriticalServiceAfter | valor em segundos que especifica que as verificações associadas a um serviço devem cancelar o registro após esse tempo |

Depois da configuração acima deve-se adicionar o SDK na aplicação,
segue abaixo a linha de código que deve ser adicionada no arquivo "Startup.cs":

```csharp
using Hubee.ServiceDiscovery.Sdk.Core.Extensions;

public class Startup
{
  //(...)
  public void ConfigureServices(IServiceCollection services)
  {
    services.AddServiceDiscovery(Configuration);
  }
}
```
