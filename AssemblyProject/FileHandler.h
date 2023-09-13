

#ifndef ASSEMBLYPROJECT_FILEHANDLER_H
#define ASSEMBLYPROJECT_FILEHANDLER_H

#endif //ASSEMBLYPROJECT_FILEHANDLER_H

#include <stdio.h>
#include <string.h>

struct FileHandler {
    const char filepath;
    int fileLines;




};

int fileLines = 0;

int FileReadPerLine(char *filepath) {
    FILE    *textfile;
    char    line[fileLines];
    char content[fileLines];
    int i = 0;

    textfile = fopen(filepath, "r");
    if(textfile == NULL)
        return 1;

    while(fgets(line, fileLines, textfile)){
        content[i] = line[i];
        i = i + 1;
    }

    fclose(textfile);
    return 0;
}

int FileLineCounter(char *fileName){
    FILE *fileptr;
    char chr;
    int count_lines = 0;

    fileptr = fopen(fileName, "r");
    //extract character from file and store in chr
    chr = getc(fileptr);
    while (chr != EOF)
    {
        //Count whenever new line is encountered
        if (chr == 'n')
        {
            fileLines = fileLines + 1;
        }
        //take next character from file.
        chr = getc(fileptr);
    }
    fclose(fileptr); //close file.

    return count_lines;
}

int FileSaveAsBinaryHack(char *fileName){
    FILE *fileptr;
    char *filePath;
    unsigned char buffer[16];

    strcat(filePath, fileName);
    strcat(filePath, ".hack");

    fileptr = fopen(filePath, "w+");

    fwrite(buffer,sizeof(buffer),1,fileptr); // write 16 bytes from our buffer

    return 0;
}

