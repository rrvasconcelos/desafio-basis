# Estágio de compilação
FROM node:20-alpine AS build
WORKDIR /app

# Copiar arquivos de configuração
COPY package.json package-lock.json ./

# Instalar dependências
RUN npm ci

# Copiar o código-fonte
COPY . .

# Compilar a aplicação
RUN npm run build -- --configuration production

# Estágio de produção
FROM nginx:alpine
WORKDIR /usr/share/nginx/html

# Copiar a aplicação compilada do estágio de compilação
COPY --from=build /app/dist/book-store/browser/ .

# Copiar configuração personalizada do nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expor a porta 80
EXPOSE 80

# Comando para iniciar o nginx
CMD ["nginx", "-g", "daemon off;"]
