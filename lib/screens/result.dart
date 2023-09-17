import 'dart:convert';
import 'dart:typed_data';
import 'package:audioplayers/audioplayers.dart';
import 'dart:io';
import 'package:flutter/material.dart';

class MediBrainResult extends StatelessWidget {
  MediBrainResult(
      {super.key,
      required this.base64AudioFile,
      required this.translation,
      required this.actualText});
  final String base64AudioFile;
  final String translation;
  final String actualText;
  AudioPlayer audioPlayer = AudioPlayer();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text("Medicine Result Page")),
      body: Center(
        child: Column(mainAxisAlignment: MainAxisAlignment.center, children: [
          ClipRRect(
            borderRadius: BorderRadius.circular(200.0),
            child: Image.file(
              File("assets/images/MedBrAIny.png"),
              width: 300,
              height: 300,
            ),
          ),
          Text("Original Text: $actualText"),
          const SizedBox(
            height: 20.0,
          ),
          Text("Translated Text: $translation"),
          const SizedBox(
            height: 20.0,
          ),

          // Text("Audio File: $base64AudioFile"),
          FloatingActionButton(
            onPressed: playAudio,
            child: const Icon(Icons.audio_file),
          )
        ]),
      ),
    );
  }

  void playAudio() async {
    Uint8List audioBytes = base64Decode(base64AudioFile);
    // await audioPlayer.setSourceBytes(audioBytes);
    await audioPlayer.play(BytesSource(audioBytes));
    // int result = await audioPlayer.playBytes(audioBytes);
  }
}
