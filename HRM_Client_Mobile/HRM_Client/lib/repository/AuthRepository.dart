import 'dart:convert';
import 'dart:io';
import 'package:HRM_Client/models/JwtToken.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:http/http.dart' as http;

Future<JwtToken> login(String email, String password) async {
  final storage = new FlutterSecureStorage();

  final params = {
    "email": email,
    "password": password
  };

  final header = {HttpHeaders.contentTypeHeader: 'application/json'};

  final response = await http.post('http://103.153.73.107:8080/api/Auth/Login',
    body: json.encode(params),
    headers: header
  );

  if (response.statusCode == 200) {
    final responseJson = jsonDecode(response.body);
    await storage.write(key: 'jwt', value: responseJson['accessToken']);

    var token = await storage.read(key: 'jwt');
    print(token);
    return JwtToken.fromJson(responseJson);
  } else {
    print(response.statusCode);
  }
}