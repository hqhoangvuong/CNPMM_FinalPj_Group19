import 'package:flutter/material.dart';
import 'package:flutter/rendering.dart';
import 'package:HRM_Client/components/rounded_button.dart';
import 'package:HRM_Client/Screens/Welcome/components/bg.dart';
import 'package:HRM_Client/Screens/Login/login_screen.dart';

class Body extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    Size size = MediaQuery.of(context).size;
    return Background(
        child: Column(
      mainAxisAlignment: MainAxisAlignment.center,
      children: <Widget>[
        SizedBox(
          height: size.height * 0.7,
        ),
        RoundedButton(
          text: "SIGN IN",
          press: () {
            Navigator.push(
              context,
              MaterialPageRoute(builder: (context) {
                return LoginScreen();
              }),
            );
          },
        ),
        RoundedButton(
          text: "SIGN IN WITH GOOGLE",
          press: () {},
          color: Colors.blue,
        )
      ],
    ));
  }
}
