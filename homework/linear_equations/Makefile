Out.txt : main.exe
	mono $< > $@

main.exe : main.cs vector.dll matrix.dll qrgs.dll
	mcs -target:exe -out:$@ -reference:vector.dll -reference:matrix.dll -reference:qrgs.dll $<

matrix.dll : matrix.cs
	mcs -target:library -out:$@ $<

vector.dll : vector.cs matrix.dll
	mcs -target:library -reference:matrix.dll -out:$@ $<

qrgs.dll : qrgs.cs matrix.dll vector.dll
	mcs -target:library -reference:matrix.dll -reference:vector.dll -out:$@ $<

clean :
	$(RM) main.exe Out.txt matrix.dll vector.dll
