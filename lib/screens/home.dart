import 'dart:io';
import 'package:flutter/material.dart';
import 'package:MedBrAIny/screens/picture.dart';

class TodoList extends StatefulWidget {
  const TodoList({super.key});

  @override
  State<TodoList> createState() => _TodoListState();
}

class _TodoListState extends State<TodoList> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: const Text('Your Medicine Buddy'),
          centerTitle: true,
        ),
        backgroundColor: Color.fromRGBO(255, 255, 255, 1),
        body: Center(
            child: Container(
          alignment: Alignment.center,
          child: Column(mainAxisAlignment: MainAxisAlignment.center, children: [
            ClipRRect(
              borderRadius: BorderRadius.circular(1500.0),
              child: Image.file(
                File("assets/images/MedBrAIny.png"),
                // width: 100,
                // height: 100,
              ),
            ),
          ]),
        )),
        floatingActionButton: FloatingActionButton(
          onPressed: () {
            final route = MaterialPageRoute(
              builder: (context) => Picture(),
            );
            Navigator.push(context, route);
          },
          child: const Icon(Icons.add),
        ));
  }
}
