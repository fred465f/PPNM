N=100000000
TIME = time --portability --append --output $@

Out.txt : main.exe main2.exe Makefile
	> $@
	$(TIME) mono $< -terms:$N -threads:1 >> $@
	$(TIME) mono $< -terms:$N -threads:2 >> $@
	$(TIME) mono $< -terms:$N -threads:3 >> $@
	$(TIME) mono $< -terms:$N -threads:4 >> $@
	$(TIME) mono main2.exe -terms:$N >> $@

main.exe : main.cs
	mcs -target:exe -out:main.exe $<

main2.exe : main2.cs
	mcs -target:exe -out:main2.exe $<

clean :
	$(RM) main.exe main2.exe Out.txt
