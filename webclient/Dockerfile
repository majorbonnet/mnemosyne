FROM node:23-slim AS base
ENV PNPM_HOME="/pnpm"
ENV PATH="$PNPM_HOME:$PATH"
RUN corepack enable
COPY . /app
WORKDIR /app

FROM base AS prod-deps
RUN --mount=type=cache,id=pnpm,target=/pnpm/store pnpm install --prod --frozen-lockfile

FROM base AS build
RUN --mount=type=cache,id=pnpm,target=/pnpm/store pnpm install --frozen-lockfile
RUN pnpm run build

FROM nginx:1.27.4

COPY ./nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=prod-deps /app/node_modules /var/www/node_modules/
COPY --from=build /app/dist /var/www/html/

EXPOSE 3000
ENTRYPOINT ["nginx","-g","daemon off;"]
