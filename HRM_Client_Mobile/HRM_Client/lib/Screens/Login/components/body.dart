import 'package:HRM_Client/repository/AuthRepository.dart';
import 'dart:developer';
import 'package:flutter/material.dart';
import 'package:flutter/rendering.dart';
import 'package:HRM_Client/components/rounded_input_field.dart';
import 'package:HRM_Client/components/rounded_button.dart';
import 'package:HRM_Client/components/rounded_password_field.dart';
import 'package:HRM_Client/Screens/Welcome/components/bg.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class Body extends StatelessWidget {
  TextEditingController emailController = new TextEditingController();
  TextEditingController passwordController = new TextEditingController();

  @override
  Widget build(BuildContext context) {
    Size size = MediaQuery.of(context).size;
    return Background(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            SizedBox(
            height: size.height * 0.35,
        ),
            RoundedInputField(
              hintText: "Your Email",
              controller: emailController,
              onChanged: (value) { },
        ),
            RoundedPasswordField(
              controller: passwordController,
              onChanged: (value) { },
        ),
            SizedBox(height: size.height * 0.05),
            RoundedButton(
          text: "SIGN IN",
          press: () => checkLogin(emailController.text, passwordController.text),
            )
          ],
        ));
  }

  Future<void> checkLogin(String username, String password) async {
    final storage = new FlutterSecureStorage();
    print(emailController.text);
    var response = login(username, password);

    // final key = await storage.read(key: 'jwt');
    log('data: $response');
  }
}
