FROM node:lts-alpine AS build
WORKDIR /src
COPY package*.json /src/
RUN npm install
COPY . .
RUN npm run build

FROM node:lts-alpine AS release
WORKDIR /app
COPY --from=build /src .

EXPOSE 8080
EXPOSE 80
CMD [ "npm", "run", "serve" ]