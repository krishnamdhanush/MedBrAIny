using Azure;
using Azure.AI.OpenAI;

namespace MedBrainyAPI.Services
{
    public class OpenAIService
    {
        string key = "your-openai-key";
        string endpoint = "https://tdei-openai.openai.azure.com/";

        string prompt = @"Extract the expiry date from the text

                        Here are some examples - 
                        
                        Input - Expiry Date - 19/11/2021
                        Output - 19/11/2021

                        Input - Mgf Dt - 1/11/2021 Exp Dt - 1/11/2024
                        Output - 1/11/2024

                        Input - Mgf Dt - 1/11/2021 Use before 19/11/2024
                        Output - 19/11/2022

                        Input - Mgf Dt - 21/11/2021 Best before 7/11/2024
                        Output - 7/11/2022

                        Input - Mgf Dt - 10 September 2021 Expiry 19 November 2024
                        Output - 19/11/2024

                        Just output the date part only in dd/MM/YYYY format. Do not output any logic/code to extract it. For example, if the date is 19/11/2021, just output 19/11/2021 and not any code or logic that can be used to extract the date.

                        If the expiry date is not mentioned in the text, output ""Expiry Date not found"".

                        Note:

                        - The text contains dates other than the expiry date, but the expiry date will always be mentioned like in the eamples above.
                        - The text may contain multiple dates, but the expiry date will be mentioned only once. 

                        Input - 
                        ";
        string deploymentName = "TDE-ChatGpT";

        public string GetExpiryDate(string ocrText)
        {
            var client = new OpenAIClient(new Uri(endpoint), new AzureKeyCredential(key));
    
            var conversationMessages = new ChatMessage(
                ChatRole.User, prompt + ocrText + "\n");
            var chatCompletionsOptions = new ChatCompletionsOptions();

            chatCompletionsOptions.Messages.Add(conversationMessages);

            conversationMessages = new ChatMessage(
            ChatRole.Assistant, "Output - \n");


            chatCompletionsOptions.Messages.Add(conversationMessages);

            Response<ChatCompletions> completionsResponse = client.GetChatCompletions(deploymentName, chatCompletionsOptions);
            string completion = completionsResponse.Value.Choices[0].Message.Content;
            return completion;
        }
    }
}
