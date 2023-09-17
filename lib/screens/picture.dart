import 'package:camera/camera.dart';
import 'package:flutter/material.dart';
import 'package:MedBrAIny/screens/camera.dart';
// import 'package:todo/screens/camera.dart';

class Picture extends StatefulWidget {
  const Picture({super.key});

  @override
  State<Picture> createState() => _PictureState();
}

class _PictureState extends State<Picture> {
  @override
  Widget build(BuildContext context) {
    // return Scaffold(
    //     appBar: AppBar(
    //       title: const Text('Add Todo'),
    //       centerTitle: true,
    //     ),
    //     body: ListView(
    //       padding: const EdgeInsets.all(10.0),
    //       children: [
    //         const TextField(
    //           decoration: InputDecoration(
    //             labelText: 'Title',
    //             hintText: 'Enter your todo title',
    //           ),
    //         ),
    //         const TextField(
    //           keyboardType: TextInputType.multiline,
    //           minLines: 3,
    //           maxLines: 5,
    //           decoration: InputDecoration(
    //             labelText: 'Description',
    //             hintText: 'Enter your todo description',
    //           ),
    //         ),
    //         const SizedBox(
    //           height: 20.0,
    //         ),
    //         ElevatedButton(onPressed: Picture, child: Text('Add todo'))
    //       ],
    //     ));
    return Scaffold(
      appBar: AppBar(title: const Text("Home Page")),
      body: SafeArea(
        child: Center(
            child: ElevatedButton(
          onPressed: () async {
            // print("hello");
            await availableCameras().then((value) => {
                  Navigator.push(
                      context,
                      MaterialPageRoute(
                          builder: (_) => CameraPage(cameras: value.first)))
                });
          },
          child: const Text("Take a Picture"),
        )),
      ),
    );
  }

  void Picture() {}
}
