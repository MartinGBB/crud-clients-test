# Customer CRUD API com Testes Automatizados
Este projeto foi desenvolvido como parte do bootcamp da Trybe de C#, com o objetivo de praticar a criação de uma API CRUD e realizar testes automatizados.
A Trybe forneceu arquivos de configuração do projeto, incluindo interfaces e bases de dados, enquanto eu desenvolvi a lógica da API, controllers e a implementação dos testes para os métodos CRUD.

## Tecnologias Utilizadas
- ASP.NET Core para o desenvolvimento da API.
- XUnit para testes.
- Moq para simular o comportamento do repositório.
- AutoBogus para gerar dados fictícios nos testes.

## Testes
Os testes cobrem os principais fluxos de operação da API, utilizando mocks para simular o comportamento do repositório:

- GetAllTest(): Testa a recuperação de todos os clientes.
- GetByIdTest(): Testa a recuperação de um cliente específico e o comportamento quando o cliente não é encontrado.
- CreateTest(): Testa a criação de um cliente e a verificação dos valores retornados.
- UpdateTest(): Testa a atualização de um cliente e a resposta apropriada.
- DeleteTest(): Testa a exclusão de um cliente e o comportamento quando o cliente não é encontrado.

## Executando a Aplicação
1. Clone o repositório:
    ```
    git clone git@github.com:MartinGBB/crud-clients-test.git
    ```
2. Navegue até o diretório do projeto:
    ```
    crud-clients-test/src/CustomerCrud
    ```
3. Restaure as dependências:
    ```
    dotnet restore
    ```
4. Execute a aplicação:
    ```
    dotnet run
    ```

## Executando os Testes

1. Navegue até o diretório do projeto:
    ```
    crud-clients-test/src/CustomerCrud.Test
    ```
2. Para executar os testes, utilize o seguinte comando:
    ```
    dotnet test
    ```