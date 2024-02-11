Out.txt : main.exe
	mono $< -output:outfile_erf.txt/outfile_gamma.txt/outfile_lngamma.txt

main.exe : main.cs
	mcs -target:exe -out:$@ $<

clean :
	$(RM) main.exe outfile_erf.txt outfile_gamma.txt outfile_lngamme.txt Out.txt

