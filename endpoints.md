# endpoints

URL_API = http://localhost:5137

Authentication:

POST {{host}}/api/Authentication/login
Content-Type:  application/json

{
  "email": "email@example.com",
  "password": "Password"
}


# Tabla rápida de loggs


| Nivel       | Cuándo usarlo                           |
| ----------- | --------------------------------------- |
| Trace       | TODO detalle interno                    |
| Debug       | Info técnica de desarrollo              |
| Information | Flujo normal (lo que ya haces)          |
| Warning     | Algo raro pero tolerable                |
| Error       | Fallo controlado                        |
| Critical    | Fallo grave (posible caída del sistema) |
