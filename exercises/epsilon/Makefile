Out.txt : epsilon.exe
	mono epsilon.exe > Out.txt

epsilon.exe : epsilon.cs
	mcs -out:epsilon.exe epsilon.cs

clean :
	$(RM) *.exe [Oo]*.txt