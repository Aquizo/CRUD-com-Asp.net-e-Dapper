# CRUD com Dapper e Asp.net

Projeto de um CRUD usando Dapper para comunicação com o banco de dados PostgreSQL e o Framework Asp.net, da linguagem Backend C#(C Sharp), para renderizar as páginas Web.
###

Para o CRUD funcionar são necessários fazer dois passos:

**Passo Um**: Criar um Banco de Dados PostgreSQL com o nome pessoaDB e usar o código de comando:

CREATE TABLE pessoas(

        pessoaid SERIAL PRIMARY KEY,

        nome varchar(100),

        idade int,

        peso float
);

**Passo Dois**: No arquivo PessoasController.cs na variável ConnectionString coloque seu nome de usuário e senha para acessar o banco de dados do seu computador.




