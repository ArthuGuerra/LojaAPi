# 🛒 API de Simulação de um Sistema de Mercado

## 📌 Descrição  
Este projeto foi desenvolvido para fins de estudo, permitindo a aplicação prática dos meus conhecimentos. Ele utiliza uma arquitetura **modular**, garantindo a separação adequada das responsabilidades entre os diferentes módulos.  

## ⚙️ Funcionalidades  

### 🏷️ Gerenciamento de Produtos e Categorias  
- Conectado a um banco de dados **MySQL**.  
- Uma **categoria** pode conter vários produtos.  
- Cada **produto** pertence a apenas **uma** categoria.  

### 🔐 Sistema de Autenticação  
- Implementado com **JWT Bearer**.  
- O usuário precisa estar cadastrado para gerar um **token JWT**.  
- Apenas endpoints autorizados podem ser acessados com um **token válido**.  
- Se o usuário não tiver permissão, a requisição será negada.  

## 🛠️ Tecnologias Utilizadas  
- **C#** e **.NET**  
- **Entity Framework Core**  
- **MySQL**  
- **JWT para autenticação**  

## 🚀 Como Executar o Projeto  

### 🔧 Pré-requisitos  
- .NET 8 instalado  
- MySQL configurado  
- Configurar a **string de conexão** no `appsettings.json`

### ▶️ Rodando a API  
1. Clone o repositório:  
   ```sh
   git clone https://github.com/seu-usuario/seu-repositorio.git
