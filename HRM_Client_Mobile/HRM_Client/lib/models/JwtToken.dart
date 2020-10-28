class JwtToken{
  int id;
  String tokenType;
  String accessToken;
  DateTime expiredTime;

  JwtToken({this.id, this.tokenType, this.accessToken, this.expiredTime});

  factory JwtToken.fromJson(Map<String, dynamic> json) {
    return JwtToken(
        id: json["id"],
        tokenType: json["tokenType"],
        accessToken: json["accessToken"],
        expiredTime: DateTime.parse(json["expiredTime"])
    );
  }
}