package main

import (
	"fmt"
	"log"
	"net/http"
	"os"
	"strings"

	"github.com/google/uuid"
)

var instanceId string

func getResponse(w http.ResponseWriter, r *http.Request) {
	var payload = strings.TrimPrefix(r.URL.Path, "/")
	fmt.Fprintf(w, "GO HTTP Reply from: %s - message: %s", instanceId, payload)
}

func main() {
	var port = os.Getenv("GO_PORT")

	instanceId = uuid.New().String()

	log.Printf("Running with instance ID: %s", instanceId)
	log.Printf("Running on port %s", port)

	mux := http.NewServeMux()

	mux.HandleFunc("/", getResponse)
	log.Fatal(http.ListenAndServe(":"+port, mux))
}
