LIBS = matrix.dll vector.dll
LIBSREF = -reference:matrix.dll -reference:vector.dll

Out.txt : main.exe
	mono $< > $@

main.exe : main.cs evd.dll $(LIBS)
	mcs -target:exe -reference:evd.dll $(LIBSREF) -out:$@ $<

evd.dll : evd.cs $(LIBS)
	mcs -target:library $(LIBSREF) -out:$@ $<

clean :
	$(RM) main.exe Out.txt evd.dll