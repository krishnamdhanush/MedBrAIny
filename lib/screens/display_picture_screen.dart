import 'dart:convert';
import 'dart:io';
import 'dart:typed_data';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:http/http.dart';
import 'package:loading_animation_widget/loading_animation_widget.dart';
import 'package:MedBrAIny/screens/result.dart';

class BrainyResult {
  final String translatedText;
  final String audioBase64;
  final String originalText;

  const BrainyResult({
    required this.translatedText,
    required this.audioBase64,
    required this.originalText,
  });

  factory BrainyResult.fromJson(Map<String, dynamic> json) {
    return BrainyResult(
      translatedText: json['translatedText'],
      audioBase64: json['audioBase64'],
      originalText: json['originalText'],
    );
  }
}

String lang = "English";

class DisplayPictureScreen extends StatefulWidget {
  final String imagePath;
  bool loading = false;
  DisplayPictureScreen({super.key, required this.imagePath});

  @override
  State<DisplayPictureScreen> createState() => _DisplayPictureScreenState();
}

class _DisplayPictureScreenState extends State<DisplayPictureScreen> {
  final url = "https://localhost:7162/ImageProcessor";

  void changeLang(String value) {
    lang = value;
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(title: const Text('Display the Picture')),
        // The image is stored as a file on the device. Use the `Image.file`
        // constructor with the given path to display the image.
        body: Column(
          mainAxisAlignment: MainAxisAlignment.start,
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.start,
              children: [
                Image.file(
                  File(widget.imagePath),
                  width: MediaQuery.of(context).size.width * 0.7,
                  height: MediaQuery.of(context).size.width * 0.5,
                ),
                const SizedBox(
                  width: 50,
                ),
                DropdownButtonExample(
                  callback: changeLang,
                ),
                // const Text("hello"),
              ],
            ),
            widget.loading
                ? LoadingAnimationWidget.twistingDots(
                    leftDotColor: const Color(0xFF1A1A3F),
                    rightDotColor: const Color(0xFFEA3799),
                    size: 50,
                  )
                : Row(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      ElevatedButton(
                          child: const Text("Go back"),
                          onPressed: () {
                            Navigator.pop(context);
                          }),
                      const SizedBox(
                        width: 40,
                      ),
                      ElevatedButton(
                          child: const Text("Accept"),
                          onPressed: () async {
                            sendImage();
                          }),
                    ],
                  ),
          ],
        ));
  }

  void sendImage() async {
    setState(() {
      widget.loading = true;
    });
    File imagefile = File(widget.imagePath); //convert Path to File
    Uint8List imagebytes = await imagefile.readAsBytes(); //convert to bytes
    String base64string =
        base64.encode(imagebytes); //convert bytes to base64 string
    Response temp = await http.post(
      Uri.parse(url),
      // body: base64string,
      body: jsonEncode(<String, String>{
        'base64Img': base64string,
        'language': lang,
      }),
      headers: <String, String>{
        'Content-Type': 'application/json; charset=UTF-8',
        'accept': '*/*'
      },
    ); //send to server
    setState(() {
      widget.loading = false;
    });
    if (temp.statusCode == 200) {
      var brainyResult = BrainyResult.fromJson(jsonDecode(temp.body));
      await Navigator.of(context).push(
        MaterialPageRoute(
          builder: (context) => MediBrainResult(
            base64AudioFile: brainyResult.audioBase64,
            translation: brainyResult.translatedText,
            actualText: brainyResult.originalText,
          ),
        ),
      );
    }
  }
}

class DropdownButtonExample extends StatefulWidget {
  const DropdownButtonExample({super.key, required this.callback});
  final Function(String) callback;
  @override
  State<DropdownButtonExample> createState() => _DropdownButtonExampleState();
}

const List<String> list = <String>['English', 'Hindi', 'Telugu', 'Tamil'];

class _DropdownButtonExampleState extends State<DropdownButtonExample> {
  String dropdownValue = list.first;

  @override
  Widget build(BuildContext context) {
    return Column(
      mainAxisAlignment: MainAxisAlignment.end,
      children: [
        const Text("Select Language: "),
        DropdownButton<String>(
          value: dropdownValue,
          icon: const Icon(Icons.arrow_downward),
          elevation: 16,
          style: const TextStyle(color: Color.fromARGB(255, 254, 254, 255)),
          underline: Container(
            height: 2,
            color: Colors.primaries.first,
          ),
          onChanged: (String? value) {
            // This is called when the user selects an item.
            setState(() {
              dropdownValue = value!;
              widget.callback(dropdownValue);
            });
          },
          items: list.map<DropdownMenuItem<String>>((String value) {
            return DropdownMenuItem<String>(
              value: value,
              child: Text(value),
            );
          }).toList(),
        ),
      ],
    );
  }
}
