Out.txt : main.exe main2.exe main3.exe
	mono main.exe -numbers:1,2,3,4,5 1>> Out.txt
	echo "1 2 3\t4\n5" | mono main2.exe 2>> Out.txt
	mono main3.exe -input:input.txt -output:output.txt

main.exe : main.cs
	mcs -target:exe -out:main.exe main.cs

main2.exe : main2.cs
	mcs -target:exe -out:main2.exe main2.cs

main3.exe : main3.cs
	mcs -target:exe -out:main3.exe main3.cs

clean :
	$(RM) main.exe main2.exe main3.exe Out.txt
