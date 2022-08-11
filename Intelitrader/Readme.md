Para criar a imagem da apliacação de o comando dentro do diretorio

"docker build -f Dockerfile -t intelitrader .."

Para rodar os containers tenho o arquivo stack.yml e 
insira o comando abaixo 

docker-compose -f stack.yml up

acesse o endereço para ver os endpoints e conferir se está funcionando

http://localhost:8001/swagger/index.html

O protocolo deve ser o http

 