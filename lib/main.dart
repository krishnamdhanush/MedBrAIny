import 'package:flutter/material.dart';
import 'package:MedBrAIny/screens/home.dart';

void main() {
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Your Medicine Buddy',
      debugShowCheckedModeBanner: false,
      theme: ThemeData.dark(),
      home: const TodoList(),
    );
  }
}
