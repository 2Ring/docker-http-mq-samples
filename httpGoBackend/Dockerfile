FROM golang:1.18-alpine

WORKDIR /app

# Copy everything
COPY . ./

RUN go mod download

RUN go build -o /httpGoBackend

ENTRYPOINT ["/httpGoBackend"]