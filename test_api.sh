#!/bin/bash

BASE_URL="https://localhost:5001/api"

echo "== TEST AUTHORS =="
echo "GET ALL Authors"
curl -k -X GET "$BASE_URL/authors"
echo -e "\n"

echo "POST new Author"
curl -k -X POST "$BASE_URL/authors" \
    -H "Content-Type: application/json" \
    -d '{"name":"New Author"}'
echo -e "\n"

echo "GET Author by ID 1"
curl -k -X GET "$BASE_URL/authors/1"
echo -e "\n"

echo "PUT update Author ID 1"
curl -k -X PUT "$BASE_URL/authors/1" \
    -H "Content-Type: application/json" \
    -d '{"id":1,"name":"Updated Author"}'
echo -e "\n"

echo "DELETE Author ID 1"
curl -k -X DELETE "$BASE_URL/authors/1"
echo -e "\n"


echo "== TEST BOOKS =="
echo "GET ALL Books"
curl -k -X GET "$BASE_URL/books"
echo -e "\n"

echo "POST new Book"
curl -k -X POST "$BASE_URL/books" \
    -H "Content-Type: application/json" \
    -d '{"title":"New Book","authorId":2,"categoryId":2}'
echo -e "\n"

echo "GET Book by ID 1"
curl -k -X GET "$BASE_URL/books/1"
echo -e "\n"

echo "PUT update Book ID 1"
curl -k -X PUT "$BASE_URL/books/1" \
    -H "Content-Type: application/json" \
    -d '{"id":1,"title":"Updated Book","authorId":2,"categoryId":2}'
echo -e "\n"

echo "DELETE Book ID 1"
curl -k -X DELETE "$BASE_URL/books/1"
echo -e "\n"


echo "== TEST CATEGORIES =="
echo "GET ALL Categories"
curl -k -X GET "$BASE_URL/categories"
echo -e "\n"

echo "POST new Category"
curl -k -X POST "$BASE_URL/categories" \
    -H "Content-Type: application/json" \
    -d '{"name":"New Category"}'
echo -e "\n"

echo "GET Category by ID 1"
curl -k -X GET "$BASE_URL/categories/1"
echo -e "\n"

echo "PUT update Category ID 1"
curl -k -X PUT "$BASE_URL/categories/1" \
    -H "Content-Type: application/json" \
    -d '{"id":1,"name":"Updated Category"}'
echo -e "\n"

echo "DELETE Category ID 1"
curl -k -X DELETE "$BASE_URL/categories/1"
echo -e "\n"

echo "== DONE =="
