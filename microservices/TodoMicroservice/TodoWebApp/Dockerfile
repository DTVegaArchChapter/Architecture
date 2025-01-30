FROM node:16-alpine AS build-env
WORKDIR /app

COPY TodoWebApp/ ./

RUN npm install
RUN npm run build

FROM nginx
RUN rm /etc/nginx/nginx.conf /etc/nginx/mime.types
COPY TodoWebApp/nginx.conf /etc/nginx
COPY TodoWebApp/mime.types /etc/nginx
ENV TZ=Europe/Istanbul
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
WORKDIR /usr/share/nginx/html
COPY --from=build-env /app/dist .
EXPOSE 80