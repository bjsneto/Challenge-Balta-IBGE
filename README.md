
#  Desafio do Balta

## Descrição do Projeto

Este projeto foi desenvolvido como parte de um desafio de construção de uma API utilizando o conceito de Minimal API com base nos dados de código, cidade e estado do Brasil disponíveis no repositório [IBGE](https://github.com/andrebaltieri/ibge). O objetivo era criar uma API que oferecesse funcionalidades de autenticação, CRUD de localidades, pesquisa por cidade, estado, código do IBGE e importação de dados a partir de um arquivo Excel.

## Tecnologias Utilizadas

- **Plataforma:** .NET 7
- **Arquitetura:** Minimal APIs
- **Banco de Dados:** PostgreSQL 
- **Testes:** Testes de unidade usando NUnit


## Funcionalidades

### Autenticação e Autorização

A API oferece autenticação e autorização, garantindo a segurança das operações. Para acessar endpoints protegidos, é necessário fazer login e obter um token JWT.

-   **Cadastro de E-mail e Senha:** Os usuários podem se cadastrar fornecendo um e-mail e senha.
    
-   **Login (Token, JWT):** Os usuários autenticados recebem um token JWT, que deve ser incluído no cabeçalho das solicitações para acessar endpoints protegidos.

### CRUD de Localidade

A API permite a criação, pesquisa, atualização e exclusão de localidades, incluindo código, estado e cidade. 

-   **Criação de Localidade:** Crie novas localidades fornecendo informações como código, estado e cidade.
    
-   **Atualização de Localidade:** Atualize as informações de uma localidade existente.
    
-   **Exclusão de Localidade:** Exclua uma localidade existente.

-   **Pesquisa de Localidade:** Pesquisa localidades por código, estado e cidade

### Importação de Dados

A API permite a importação de dados a partir de um arquivo Excel. Os dados fornecidos no arquivo serão carregados na API e estarão disponíveis para consulta.

-   **Importação de Dados a partir do Excel:** Faça o upload do arquivo Excel contendo os dados de localidades, e a API irá processar e armazenar esses dados para uso posterior.

## Boas Práticas da API

Este projeto segue boas práticas de desenvolvimento de APIs:
    
-   **Padronização:** A API segue convenções de nomenclatura e estrutura padronizadas para facilitar a compreensão e a manutenção do código.
    
-   **Documentação (Swagger):** A API é documentada usando o Swagger, o que facilita a compreensão das rotas, parâmetros e respostas disponíveis.

## Uso da API

Para começar a usar a API, siga as etapas a seguir:

1.  **Registro de Usuário:** Crie uma conta fornecendo um e-mail e senha.
    
2.  **Login:** Faça login para obter um token JWT, que deve ser incluído no cabeçalho das solicitações autenticadas.
    
3.  **CRUD de Localidades:** Utilize as rotas para criar, atualizar, excluir e consultar localidades.
    
4.  **Pesquisas:** Realize pesquisas por cidade, estado ou código do IBGE.
    
5.  **Importação de Dados:** Faça o upload do arquivo Excel para importar dados de localidades.

## Rotas da API

A API fornece as seguintes rotas:

-   `/api/user/create`: Rota para criar de usuários.
-   `/api/user/login`: Rota para login de usuários.
-   `/api/ibge/create`: Rota para criar uma localidade
-    `/api/ibge/search`: Rota para pesquisar uma localidade por código, estado ou cidade
-   `/api/ibge/delete`: Rota excluir uma localidade
-   `/api/ibge/update`: Rota atualizar uma localidade
-   `/api/ibge/upload`: Rota para importação de dados a partir de um arquivo Excel.

## Exemplo de Requisição

Aqui está um exemplo de como fazer uma requisição para criar uma nova localidade:

    POST /api/ibge/create
    Content-Type: application/json
    Authorization: Bearer [seu-token-jwt]
    
    {
        "codigo": 3550308,
        "estado": "SP",
        "cidade": "São Paulo"
    }
	
## Documentação do Swagger

A documentação da API pode ser acessada através do Swagger, que fornece detalhes sobre todas as rotas disponíveis, parâmetros e respostas. Para acessar a documentação, basta abrir a seguinte URL no navegador:


`https://challenge-balta-ibge.onrender.com/swagger` 


##  Integração Contínua (CI) e Entrega Contínua (CD)


Neste projeto, foi adotada uma abordagem de Integração Contínua (CI) e Entrega Contínua (CD) para garantir a qualidade do código e a implantação eficiente. O processo é o seguinte:

1.  **GitHub Actions para CI/CD**: Utilizamos o GitHub Actions para criar um fluxo contínuo de implantação. Sempre que uma alteração é feita no repositório do GitHub, o GitHub Actions entra em ação. Ele realiza o seguinte:
    
    -   **Construção Automatizada**: Compila o código-fonte e realiza testes automatizados.
        
    -   **Implantação Contínua**: Após a construção bem-sucedida, a aplicação é automaticamente implantada na plataforma Render.
        
2.  **Dockerfile**: O arquivo Docker (Dockerfile) contém as instruções necessárias para empacotar o aplicativo em um contêiner, garantindo a portabilidade e consistência da aplicação em diferentes ambientes.


## Conclusão

Este projeto atende plenamente aos requisitos do desafio, proporcionando uma API funcional, segura e bem documentada. Além disso, a integração contínua (CI) e a entrega contínua (CD) garantem que as atualizações sejam implementadas de forma eficiente, mantendo a aplicação constantemente atualizada e pronta para atender às necessidades dos usuários.
    

