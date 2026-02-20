# Build stage - prepare files
FROM nginx:alpine AS builder

# Create app directory
WORKDIR /app

# Copy web files
COPY . /app/

# Production stage
FROM nginx:alpine

# Install bash for startup script
RUN apk add --no-cache bash

# Copy custom nginx configuration
COPY docker/nginx.conf /etc/nginx/conf.d/default.conf

# Copy web application
COPY --from=builder /app /usr/share/nginx/html

# Copy startup script
COPY docker/docker-entrypoint.sh /docker-entrypoint.sh
RUN chmod +x /docker-entrypoint.sh

# Expose port
EXPOSE 80

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD wget --no-verbose --tries=1 --spider http://localhost/health || exit 1

# Set entrypoint
ENTRYPOINT ["/docker-entrypoint.sh"]
CMD ["nginx", "-g", "daemon off;"]
