# Folha de Pagamento Simples

Sistema que apura folhas de pagamentos simples baseadas em um arquivo CSV informado pelo usuário e que salva essas informações em um novo CSV.  
A solução está estruturada em pastas e separada por projetos.  
Existem dois executáveis que apuram as folhas, um console e uma interface gráfica (GUI).  
O sistema usa um arquivo json que contém os parâmetros usados para os cálculos e que podem ser editados pelo usuário.

## Estrutura do CSV para ser processado

```
emp;12345678900;João da Silva;2
rub;1001;Salário Base;P;5000.00
rub;2001;Vale Transporte;D;300.00
```

## Parâmetros

A apuração da folha usa um arquivo `Parametros.json` com os parâmetros:

- Salário mínimo
- Faixas de INSS
- Faixas de IR

Esses parâmetros são lidos durante a inicialização do sistema e no caso do Console.exe temos a opção de informar o diretório ou usar o padrão.  
Sintam-se a vontade para alterar tanto o diretório quanto os parâmetros!

## Executáveis

Tanto o Console.exe quando o GUI.exe realizam os cálculos da folha e se encontram na pasta bin da raiz do projeto.

## Opcionais implementados

- Persistência de dados
- Interface gráfica (GUI)
- Suporte a múltiplos arquivos
- Configuração de parâmetros

## Atenção!

Esse sistema foi criado com .Net 8 por isso precisa de uma máquina que tenha o 8 instalado, principalmente por causa do SDK.  
Obs: Mesmo que eu use o Core para aplicações pessoais não tenho problemas em trabalhar com o Framework.

Gostaria muito de ser chamada para um entrevista técnica. De qualquer forma obrigado pela oportunidade!!

Atenciosamente,  
Djonathan Zuchi.
