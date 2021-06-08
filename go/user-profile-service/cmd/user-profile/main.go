package main

import (
	"log"
	"net/http"
)

type server struct{}

func (s *server) ServeHTTP(rs http.ResponseWriter, r *http.Request) {
	rs.Header().Set("Content-Type", "application/json")
	rs.WriteHeader(http.StatusOK)
	rs.Write([]byte(`{"message": "hello world"}`))
}

func main() {
	s := &server{}
	http.Handle("/", s)
	log.Fatal(http.ListenAndServe(":8080", nil))
}
